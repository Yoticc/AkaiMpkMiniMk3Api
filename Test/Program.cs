using AkaiMpkMiniMk3Api;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var device = new Device();
device.OnInput += input =>
{
    Console.WriteLine(input);
};

Console.ReadLine();