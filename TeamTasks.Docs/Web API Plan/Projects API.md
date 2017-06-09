# Projects Web API Calls

At this time, we have not yet developed how to obtain a token for the user. All we need to know at this point is that
the user name, user id, and roles are going to be encrypted into that token.

## Creation
### Request
```json
POST /api/project
Host: TBD
Authoriztion: Bearer xklvj23813n201jn9vncJX...

{
    "name": "The Female Flixpress",
    "startDate": "2016-12-01T12:45:00Z",
    "dueDate": "2017-12-01T11:10:00Z"
}
```
- `startDate` and `dueDate` are optional. They may be omitted.
- A project has a creator, but it is not specified in the JSON body because that will be derived from the authorization
header.
- The project status of a newly-created project is going to be set to "inactive".

### Responses
- **201**. Succesful creation, with a returned JSON body of simply `{"id":3}` where `id` is the newly-generated unique
identifier of the project.
- **400**. `["duplicate-on-create"]`. Another project with the same name already exists.
- **400**. `["invalid-start-and-due-dates"]`. When both `startDate` and `dueDate` are specified and the `dueDate` is
set to an earlier date than `startDate`
- **401**. `["unauthorized"]`. The requestor is not an administrator. Project creation can only be done by an administrator.

## Updating

### Request
```json
PUT /api/project
Host: TBD
Authoriztion: Bearer xklvj23813n201jn9vncJX...

{
    "id": 3,
    "name": "The Female Flixpress",
    "startDate": "2016-12-01T12:45:00Z",
    "dueDate": "2017-12-01T11:10:00Z",
    "projectStatusName":"inactive"
}
```
- `startDate` and `dueDate` are optional. They may be omitted.
- Compared to the `POST` request, we will now require `id` and `projectStatusName`

### Responses
- **200**. Succesful updating, no response body.
identifier of the project.
- **400**. `["duplicate-on-update"]`. Another project other than itself with the same name already exists.
- **400**. `["invalid-start-and-due-dates"]`. When both `startDate` and `dueDate` are specified and the `dueDate` is
set to an earlier date than `startDate`
- **401**. `["unauthorized"]`. The requestor is not an administrator.
