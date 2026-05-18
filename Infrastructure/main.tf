# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0.2"
    }
  }

  required_version = ">= 1.1.0"
}

provider "azurerm" {
  features {}
}

locals {
  location = "West US 2"
}

variable "ssh_public_key" {
  type = string
}

data "azurerm_resource_group" "rg" {
  name = "Portafolio_Brian"
}

resource "azurerm_virtual_network" "main" {
  name                = "portafolio-vnet"
  location            = local.location
  resource_group_name = data.azurerm_resource_group.rg.name
  address_space       = ["10.0.0.0/16"]
}

resource "azurerm_subnet" "main" {
  name                 = "portafolio-subnet"
  resource_group_name  = data.azurerm_resource_group.rg.name
  virtual_network_name = azurerm_virtual_network.main.name
  address_prefixes     = ["10.0.1.0/24"]
}

resource "azurerm_network_security_group" "main" {
  name                = "portafolio-nsg"
  location            = local.location
  resource_group_name = data.azurerm_resource_group.rg.name
}


resource "azurerm_network_security_rule" "http" {
  name = "AllowHTTP"
  priority = 100
  direction = "Inbound"
  access = "Allow"
  protocol = "Tcp"
  source_port_range = "*"
  destination_port_range = "80"
  source_address_prefix = "*"
  destination_address_prefix = "*"
  resource_group_name = data.azurerm_resource_group.rg.name
  network_security_group_name = azurerm_network_security_group.main.name
}


resource "azurerm_network_security_rule" "https" {
  name = "AllowHTTPS"
  priority = 110
  direction = "Inbound"
  access = "Allow"
  protocol = "Tcp"
  source_port_range = "*"
  destination_port_range = "443"
  source_address_prefix = "*"
  destination_address_prefix = "*"
  resource_group_name = data.azurerm_resource_group.rg.name
  network_security_group_name = azurerm_network_security_group.main.name
}


resource "azurerm_network_security_rule" "ssh" {
  name = "AllowSSH"
  priority = 120
  direction = "Inbound"
  access = "Allow"
  protocol = "Tcp"
  source_port_range = "*"
  destination_port_range = "22"
  source_address_prefix = "*"
  destination_address_prefix = "*"
  resource_group_name = data.azurerm_resource_group.rg.name
  network_security_group_name = azurerm_network_security_group.main.name
}


resource "azurerm_public_ip" "main" {
  name                = "portafolio-ip"
  location            = local.location
  resource_group_name = data.azurerm_resource_group.rg.name
  allocation_method = "Static"
  sku               = "Standard"
}


resource "azurerm_network_interface" "main" {
  depends_on = [
    azurerm_subnet.main
  ]

  name                = "portafolio-nic"
  location            = local.location
  resource_group_name = data.azurerm_resource_group.rg.name

  ip_configuration {
    name                          = "internal"
    subnet_id                     = azurerm_subnet.main.id
    private_ip_address_allocation = "Dynamic"
    public_ip_address_id          = azurerm_public_ip.main.id
  }
}

resource "azurerm_network_interface_security_group_association" "main" {
  network_interface_id = azurerm_network_interface.main.id
  network_security_group_id = azurerm_network_security_group.main.id
}


resource "azurerm_linux_virtual_machine" "main" {
  name = "portafolio-vm"
  resource_group_name = data.azurerm_resource_group.rg.name
  location = local.location
  size = "Standard_D2s_v3"
  admin_username = "brian"

  network_interface_ids = [
    azurerm_network_interface.main.id
  ]

  tags = {
    Project     = "Portafolio"
    Environment = "Production"
  }

  admin_ssh_key {
    username = "brian"
    public_key = var.ssh_public_key
  }

  os_disk {
    caching = "ReadWrite"
    storage_account_type = "Standard_LRS"
  }

  source_image_reference {
    publisher = "Canonical"
    offer = "0001-com-ubuntu-server-jammy"
    sku = "22_04-lts"
    version = "latest"
  }
  disable_password_authentication = true
}

output "vm_ip" {
  value = azurerm_public_ip.main.ip_address
}

output "ssh_user" {
  value = azurerm_linux_virtual_machine.main.admin_username
}

output "ssh_command" {
  value = "ssh ${azurerm_linux_virtual_machine.main.admin_username}@${azurerm_public_ip.main.ip_address}"
}
