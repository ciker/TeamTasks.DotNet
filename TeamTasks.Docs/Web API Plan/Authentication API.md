# Authentication API

## Obtaining the Access Token
We will require access tokens for nearly every API request. The access token encrypts the username and
roles of the user, as well as the date that the access token was issued.

### Request
```
POST /token HTTP/1.1
host: localhost:59829
Content-Type: application/x-www-form-urlencoded

username=admin&password=Aaa000$&grant_type=password
```

We make the token request to the Auth Server. The in-solution host for the Auth Server is
localhost:59829.

Creating a separate authentication server from the resource server may seem unnecessary for
this application, but in case this application grows into a multi-company configuration, it
is ready.

### Responses
- **200**. Successful authentication.
  ```json
  {
    "roles":["admin","member"],
    "access_token":"oVzT6clNU4zi+ouWM7mTsLGgr2Wx+nvxBZE..."
  }
  ```
  We supply the roles so that a front-end application can perform some UI logic depending on the user's role[s].
- **401**. `["invalid-user-credentials"]`. The password was wrong or the user doesn't exist.

## Users In The System
At this time, we have the following users in the system: admin, user1, user2, user3, user4, and user5.
All of them are in the member role. Only the user admin has the admin role. All of them have the same
password Aaa000$.


