using System;
using System.Net;
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
    }
}