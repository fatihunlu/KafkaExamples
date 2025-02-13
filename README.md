# KafkaDemo: Understanding Kafka

KafkaDemo demonstrates how to use Kafka for processing "follow" events in a social media platform. It includes a Producer that sends random follow events to Kafka and a Consumer that listens for these events and stores them in MongoDB.

[Medium Link](https://unlu-fa.medium.com/kafka-series-vol-3-handling-follow-events-in-social-media-8f82084b561a)

## Features
* **Kafka Producer**: Sends random follow events to Kafka.
* **Kafka Consumer**: Listens for events and processes them.
* **MongoDB**: Stores user data with follower information.
* **Docker**: Manages Kafka and MongoDB services.

## Setup

1. Clone the repository
```
git clone --branch kafka-series-vol3 https://github.com/fatihunlu/KafkaDemo.git
cd KafkaDemo
```
2. Build and start the services with Docker Compose:
```
docker compose up -d
```
3. Run producer and consumer
```
dotnet run --project ./KafkaDemo.Producer
dotnet run --project ./KafkaDemo.Consumer

```
