using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using DecisionsFramework.Design.Flow;

namespace Zitac.IPTools
{
    [AutoRegisterMethodsOnClass(true, "Integration", "IPTools")]
    public class IPToolSteps
    {
        public bool AreIpsInSameSubnet(string ipAddress1, string ipAddress2, string subnetMask)
        {
            // Parse strings to IPAddress
            IPAddress ip1 = IPAddress.Parse(ipAddress1);
            IPAddress ip2 = IPAddress.Parse(ipAddress2);
            IPAddress mask = IPAddress.Parse(subnetMask);

            // Convert IP addresses to byte arrays
            byte[] ip1Bytes = ip1.GetAddressBytes();
            byte[] ip2Bytes = ip2.GetAddressBytes();
            byte[] maskBytes = mask.GetAddressBytes();

            // Check if both IPs are in the same subnet
            for (int i = 0; i < ip1Bytes.Length; i++)
            {
                if ((ip1Bytes[i] & maskBytes[i]) != (ip2Bytes[i] & maskBytes[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public bool ValidateIPAddress(string ipAddress)
        {
            if (IPAddress.TryParse(ipAddress, out _))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsIpAddressInSubnet(string ipAddress, string subnetAddress, string subnetMask)
        {
            // Parse strings to IPAddress
            IPAddress ip = IPAddress.Parse(ipAddress);
            IPAddress subnet = IPAddress.Parse(subnetAddress);
            IPAddress mask = IPAddress.Parse(subnetMask);

            // Convert IP addresses to byte arrays
            byte[] ipBytes = ip.GetAddressBytes();
            byte[] subnetBytes = subnet.GetAddressBytes();
            byte[] maskBytes = mask.GetAddressBytes();

            // Check if the IP is in the subnet
            for (int i = 0; i < ipBytes.Length; i++)
            {
                if ((ipBytes[i] & maskBytes[i]) != subnetBytes[i])
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsValidSubnet(string subnetAddress, string subnetMask)
        {
            // Parse strings to IPAddress
            IPAddress subnet = IPAddress.Parse(subnetAddress);
            IPAddress mask = IPAddress.Parse(subnetMask);

            // Convert IP addresses to byte arrays
            byte[] subnetBytes = subnet.GetAddressBytes();
            byte[] maskBytes = mask.GetAddressBytes();

            // Validate if subnet is valid
            for (int i = 0; i < subnetBytes.Length; i++)
            {
                if ((subnetBytes[i] & maskBytes[i]) != subnetBytes[i])
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsValidSubnetMask(string subnetMask)
        {
            IPAddress mask = IPAddress.Parse(subnetMask);
            byte[] maskBytes = mask.GetAddressBytes();

            bool encounteredZero = false; // Flag to check if we’ve encountered a 0 bit
            for (int i = 0; i < maskBytes.Length; i++)
            {
                byte byteVal = maskBytes[i];
                for (int bit = 7; bit >= 0; bit--) // Check bits from left to right
                {
                    bool isBitSet = (byteVal & (1 << bit)) != 0;

                    if (!isBitSet) // If bit is 0
                    {
                        encounteredZero = true;
                    }
                    else if (encounteredZero) // If bit is 1 and we’ve seen a 0 bit
                    {
                        // Invalid because we have a 1 bit after a 0 bit
                        return false;
                    }
                }
            }
            return encounteredZero; // Valid if we’ve encountered at least one 0 bit
        }

    }
}