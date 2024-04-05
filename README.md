# PassIn API

O projeto é uma API de um evento criado na NLW Unite (2024), evento da Rocketseat.

## Funcionalidades

* Criação de um Evento
* Listagem de um Evento
* Criação de um Participante de um Evento
* Listagem de Participantes de um Evento
* CheckIn de um Participante

## Características

O projeto usa DDD, SQLite como Banco de Dados (em dsv) e AutoMapper para Mapeamento de Objetos.

Em breve, pretendo colocar o projeto em produção e assim alterar o banco de dados utilizado.

## Rotas

| Método | Rota                                | Parâmetro         |
| ------ | ----------------------------------- | ----------------- |
| POST   | `/api/events`                       |                   |
| GET    | `/api/events/{id}`                  | GUID `id`         |
| POST   | `/api/attendees/{eventId}/register` | GUID `eventId`    |
| GET    | `/api/attendees/{eventId}`          | GUID `eventId`    |
| POST   | `/api/checkin/{attendeeId}`         | GUID `attendeeId` |

## StatusCode Possíveis

| Rota                                | StatusCodes        |
| ----------------------------------- | ------------------ |
| `/api/events`                       | 201, 400           |
| `/api/events/{id}`                  | 200, 404           |
| `/api/attendees/{eventId}/register` | 201, 400, 404, 409 |
| `/api/attendees/{eventId}`          | 200, 404           |
| `/api/checkin/{attendeeId}`         | 201, 404, 409      |

## Dev

* [Steph Hoel](https://github.com/StephHoel)
