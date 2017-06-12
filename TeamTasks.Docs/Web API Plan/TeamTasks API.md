# TeamTasks Web API

See the Authentication API document for information on how to obtain the Bearer token.

## Creation

### Request
```json
POST /api/teamtasks HTTP/1.1
Host: localhost:59835
Authorization: Bearer xklvj23813n201jn9vncJX...
Content-Type: application/json;charset=utf-8

{
   "startDate": "2017-03-11T13:27:10Z",
   "endDate": "2017-07-10T15:30:10Z",
   "name": "Web API Plan",
   "description": "Think about what each type of user will be able to do. Design the API calls for each user's tasks, including path and request/response bodies",
   "projectId": 2,
   "parentTeamTaskId": 2
}
```
- `startDate` and `dueDate` are optional. They may be omitted.
-  The project status of a newly-created project is going to be set to "inactive".
-  `projectId` is optional. Omitting it means that we are creating a task independent of any project.
-  `parentTeamTaskId` is optional. Including it means making the task a child of that parent task.

### Responses
- **201**. Succesful creation, with a returned JSON body of simply `{"id":2}` where `id` is the newly-generated unique
identifier of the team task.
- **400**. `["invalid-start-and-due-dates"]`. When both `startDate` and `dueDate` are specified and the `dueDate` is
set to an earlier date than `startDate`
- **400**. `["project-not-found"]`. When `projectId` is specified but the project doesn't exist.
- **401**. `["unauthorized"]`. The requestor is not an administrator. Team task creation can only be done by an administrator.

## Updating

### Requests
```json
PUT /api/teamtasks HTTP/1.1
Host: localhost:59835
Authorization: Bearer xklvj23813n201jn9vncJX...
Content-Type: application/json;charset=utf-8

{
   "id": 2
   "startDate": "2017-03-11T13:27:10Z",
   "endDate": "2017-07-10T15:30:10Z",
   "name": "Web API Planning",
   "description": "Think about what each type of user will be able to do. Design the API calls for each user's tasks, including path and request/response bodies",
   "projectId": 2,
   "parentTeamTaskId": 2,
   "teamTaskStatusName": "active"
}
```
- `startDate` and `dueDate` are now optional.
- `name` and `description` are be required, but may contain empty strings.
- `id` is required to determine which team task is being updated.
- `teamTaskStatusName` is now required when editing a team teask.
- `parentTeamTaskId` will be required, however the value cannot be the value of any team task's children or any of its
descendants.

### Responses
- **200**. Succesful updating, no response body.
identifier of the project.
- **400**. `["invalid-start-and-due-dates"]`. When both `startDate` and `dueDate` are specified and the `dueDate` is
set to an earlier date than `startDate`
- **400**. `["parent-team-task-cannot-be-descendant"]`. If the supplied `parentTeamTaskid` is the `id` of one of its children
or any of its descendants.
- **400**. `["project-not-found"]`. When `projectId` is specified but the project doesn't exist.
- **401**. `["unauthorized"]`. The requestor is not an administrator.
- **404**. `["record-not-found"]`. The id specified does not exist.

## Obtaining a TeamTask Tree
Since a team task is a task with sub-tasks, we have the option of obtaining the tree of a single team task.

### Request
```json
GET /api/teamtasks/tree/3 HTTP/1.1
Host: localhost:59835
Content-Type: application/json;charset=utf-8
Authorization: Bearer xklvj23813n201jn9vncJX...
```

The api call is in the format `/api/teamtasks/tree/{id}` where `id` is the team task id.

The portion `tree` is NOT a parameter. That is required to be as-is to specify that we want a tree, not just the
editable properties of a team task.

### Responses
- **200**. Successful query
  ```json
  {
    "id":2,
    "name":"Implementation Plan",
    "children":[
        {
            "id":3,
            "name":"Data Plan",
            "children":[],
            "status":"inactive"
        },
        {
            "id":4,
            "name":"Web API Plan",
            "children":[],
            "status":"inactive"
         }
     ],
     "status":"inactive"          
   }
  ```
- **401**. `["unauthorized"]`. The requestor is not an administrator.
- **404**. `["record-not-found"]`. The id specified does not exist.
