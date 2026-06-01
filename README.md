# Chaotic Cupid Pub-Sub System

Chaotic Cupid Pub-Sub System is a distributed publish-subscribe application built with ASP.NET Core SignalR.

The system simulates a matchmaking service where users subscribe by providing personal information (username, city, age, and phone number), while a publisher component ("Cupid") periodically generates and delivers love letters between users based on a matching algorithm.

## Features

- User subscription through SignalR Hub
- Real-time message delivery
- Matchmaking algorithm
- Letter delivery every minute
- Letter confirmation mechanism
- User blocking with `/block username` command
- Prevention of duplicate letter delivery until confirmation
- Publisher/Subscriber architecture using SignalR

## Technologies

- ASP.NET Core
- SignalR
- C#
- .NET 8
- Console Applications

## Project Structure

- **ChaoticCupidPubSubSystem** – ASP.NET Core SignalR server
- **Publisher** – Cupid service responsible for sending letters
- **Subscribers** – Console client representing subscribed users
