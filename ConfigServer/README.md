# Spring Cloud Config Server for Workshop Labs

To startup a Spring Cloud Config Server, run this project as a Spring Boot app by issuing:

```
$ ./mvnw spring-boot:run
```

or

```
$ ./mvnw package
$ java -jar target/*.jar
```

It will start up on port 8888 and serve configuration data from
"file:./steeltoe/config-repo":