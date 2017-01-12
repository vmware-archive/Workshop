FROM java:8
ADD target/configserver-0.0.1-SNAPSHOT.jar app.jar
VOLUME /steeltoe/config-repo
EXPOSE 8888
ENTRYPOINT ["java","-jar","/app.jar"]