using System;
using System.IO;

namespace Connector.Client;

public class ApiException : Exception
{
    public int StatusCode { get; init; }
    public Stream? Content { get; init; }
}