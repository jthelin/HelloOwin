# Hello Owin

A simple example of a service-oriented client-server application written using [OWIN](http://owin.org)

## Build Status

Mono | Windows
---- | -------
[![Build Status](https://travis-ci.org/jthelin/HelloOwin.svg?branch=master)](https://travis-ci.org/jthelin/HelloOwin) | [![Build status](https://ci.appveyor.com/api/projects/status/qonxrrters2t5kxk?svg=true)](https://ci.appveyor.com/project/jthelin/helloowin)

## How to Run the Sample

1. Build `HelloOwin.sln`, which should restore / download any missing NuGet packages.

2. Run `HelloOwinServer.exe` (use default values, or /? to see cmd line options)

3. Run `HelloOwinClient.exe` (use default values, or /? to see cmd line options)

### Server Log Messages

If the server starts up ok, then you should see console messages similar to this:

``` console
Starting Owin server at Address = http://localhost:12345 UseJson = True

Press any key to exit.
```

### Client Log Messages

   If everything works ok for client, then you will see console messages similar to this.

``` console
Hello, Jorgen!

Press any key to exit.
```

If the server is not running,
or if the client is trying to connect to a different address than the server is listenting to,
then you will see error messages on the console window similar to this.

``` console
Error starting HelloOwinClient.exe
System.AggregateException: One or more errors occurred.
---> System.Net.WebException: Unable to connect to the remote server
---> System.Net.Sockets.SocketException: No connection could be made because the target machine actively refused it
127.0.0.1:54321
```
