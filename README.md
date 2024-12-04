# HTTP Conformity

Simple tool to check a web server for conformity with the HTTP/1.1 specification defined in RFC9110.
As a basic requirement, you need to supply an endpoint that will return a `HTTP 200 OK` status when requested
via `GET`.

## Running the Tool

You can either clone this repository and run the project using the .NET SDK or use Docker.

### .NET SDK

After cloning the repository, open a terminal and run the following commands:

```bash
cd http-conformity
dotnet run -- test https://google.com
```

### Docker

The following command runs a test via Docker, without the need of cloning this repository.
Keep in mind that the target server needs to be reachable from inside the Docker container.

```bash
docker run --it --rm genhttp/http-conformity test https://google.com
```

### Return Codes

Together with the graphical output the program will return an exit code that
indicates the result of the test:

| Exit Code | Status        | Description                                                              |
|-----------|---------------|--------------------------------------------------------------------------|
| 1         | SUCCESS       | The server did pass the test without errors.                             |
| 2         | NOT_AVAILABLE | The server did not respond to a simple HTTP GET request with status 200. |
| 3         | FAILED        | One or more validation rules did not pass.                               |
