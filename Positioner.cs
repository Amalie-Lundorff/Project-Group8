using System.Net.Sockets;
using System.Text;

const string program = @"
def f():
  p1 = p[.130, -.345, .548, 2.01, -.001, -.007]
  p2 = p[.482, -.118, .044, 3.182, -.003, -.009]
  p3 = p[.425, -.225, .044, 3.146, -.478, -.001] 
  p4 = p[.292, -.385, .044, 2.972, -1.166, -.041]
  p5 = p[.027, -.482, .044, 2.508, -1.984, -.015]
  p6 = p[ 0.482, -0.118, -.125, 3.182, -0.003, -0.009]
  p7 = p[ 0.425, -0.225, -.125, 3.146, -0.478, -0.001]
  p8 = p[ 0.292, -0.385, -.125, 2.972, -1.166, -0.041]
  p9 = p[ 0.027, -0.482, -.125, 2.508, -1.984, -0.015]

  times = 0
  while (times < 1):
    movej(get_inverse_kin(p1))
    movej(get_inverse_kin(p2))
    movej(get_inverse_kin(p3))
    movej(get_inverse_kin(p4))
    movej(get_inverse_kin(p5))
    movej(get_inverse_kin(p6))
    movej(get_inverse_kin(p7))
    movej(get_inverse_kin(p8))
    movej(get_inverse_kin(p9))
    times = times + 1
  end
end
";

const int urscriptPort = 30002, dashboardPort = 29999;
const string IpAddress = "172.20.254.201";

void SendString(string host, int port, string message)
{
    using var client = new TcpClient(host, port);
    using var stream = client.GetStream();
    stream.Write(Encoding.ASCII.GetBytes(message));
}

SendString(IpAddress, dashboardPort, "brake release\n");
SendString(IpAddress, urscriptPort, program);
// To stop:
// SendString(IpAddress, dashboardPort, "stop\n");
