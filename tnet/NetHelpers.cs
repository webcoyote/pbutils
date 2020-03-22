using System;
using System.Net;
using System.Text;

namespace TNet {
    public static class NetHelpers {
        /// <summary>
        /// Parse IP addresses in these forms:
        /// - 1.2.3.4                               IPv4
        /// - 1.2.3.4:80                            IPv4 with port
        /// - 2001:db8:0:0:0:ff00:42:8329           IPv6
        /// - [2001:db8:0:0:0:ff00:42:8329]:443     IPv6 with port
        ///
        /// If the port parameter is not valid, then the address MUST include a valid port.
        /// </summary>
        public static Tuple<string, int> ParseEndpoint (string address, int port = -1) {
            string[] quibbles = address.Split(':');
            if (quibbles.Length > 2) {
                // IPv6 address, perhaps with port
                do {
                    if (quibbles[0][0] != '[')
                        break;
                    var temp = quibbles[quibbles.Length - 2];
                    if (temp[temp.Length - 1] != ']')
                        break;
                    temp = quibbles[quibbles.Length - 1];
                    if (!int.TryParse(temp, out port))
                        return null;
                    address = string.Join(":", quibbles, 0, quibbles.Length - 1);
                } while (false);
            } else if (quibbles.Length == 2) {
                // IPv4 with port
                if (!int.TryParse(quibbles[1], out port))
                    return null;
                address = quibbles[0];
            }

            if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
                return null;
            return new Tuple<string, int>(address, port);
        }

        /// <summary>
        /// Parse a partial Uri string and convert it into a full URI string.
        /// </summary>
        /// <param name="address">A full or partial Uri</param>
        /// <param name="scheme">optional, but if invalid then there must be a scheme in the address</param>
        /// <param name="port">optional, but if invalid then there must be a port in the address</param>
        /// <returns></returns>

        public static string ParseUri (string address, string scheme = null, int port = -1) {
            var match = System.Text.RegularExpressions.Regex.Match(address, "(http|https):\\/\\/([^/]+)$");
            if (match.Success) {
                scheme = match.Groups[1].Value;
                address = match.Groups[2].Value;
            }
            if (scheme == null)
                return null;

            var addressParts = ParseEndpoint(address, port);
            if (addressParts == null)
                return null;
            return String.Format("{0}://{1}:{2}", scheme, addressParts.Item1, addressParts.Item2);
        }
    }
}


