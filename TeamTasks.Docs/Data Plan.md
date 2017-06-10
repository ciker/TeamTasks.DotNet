# TeamTasks Data Plan

## Project
A project represents a specific website or software to be developed.

- **Id (int)**. The unique identifier
- **Name (string)**. The name of the project. This must be unique across all projects.
- **CreatedDate (DateTime, required)**. When the project was first saved to data store.
- **CreatorId**. The user who created the project. Only a user with an admin role can create a project.
- **StartDate (DateTime, nullable)**. When the project is in effect.
- **DueDate (DateTime, nullable)**. When the project is due.
- **ProjectStatusId (int)**. The status of the project.

## ProjectStatus
ProjectStatus describes the state of progress of a project.

- **Id (int)**. The unique identifier
- **Name (string)**. The name of the project status. This must be unique.

The possible name values are "active", "inactive","complete", and "dropped".

## TeamTask
A TeamTask is a specific job that needs to be completed. A project may contain multiple TeamTasks. Also,
a TeamTask may be independent of a project. (We call this TeamTask instead of just "Task" because "Task" is already
a special class in C# that we use very often.)

- **Id (int)**. The unique identifier
- **Name (string)**. The name of the task. This is a unique field in combination with ProjectId.
- **CreatedDate (DateTime, required)**. When the task was first saved to data store.
- **StartDate (DateTime, nullable)**. When the task is in effect.
- **DueDate (DateTime, nullable)**. When the task is due.
- **ProjectId (int, nullable)**. The project in which the TeamTask belongs. A task may only belong to one project.
This is nullable because we will allow creation of TeamTasks that do not belong to a specific project. If ParentTeamTaskId
is specified, the ProjectId must be equal to that of the parent TeamTask's ProjectId.
- **Description (string)**. Details about the task.
- **ParentTeamTaskId (int, nullable)**. TeamTasks have a tree structure. When this value is null, the TeamTask
is the root TeamTask. If specified, the value of this field cannot be the Id of any of its descendant TeamTasks.
- **Priority (int, nullable)**. Among its siblings, the importance of a task. Lesser value means higher priority.
It is nullable because a task may be independent of a project or has no siblings. See below for the rules of this
property.
- **TeamTaskStatusId** (int, required)**. The current status of a TeamTask.

### Priority of a TeamTask
A TeamTask's priority is its importance, where lower values mean higher importance.

This property is nullable only for the case where it is not assigned to a specific project.

If a TeamTask's ParentTeamTaskId is null, and part of a project, its siblings are other TeamTasks whose
ParentTeamTaskIds are also null, then its priority is against all of those other TeamTasks.

If a TeamTask's ParentTeamTaskId is specified, (both part of a project or independent of a project), its
priority is against all other TeamTasks whose ParentTeamTaskIds are the same.

When either of the two cases above are met for a TeamTask, it is said to be a sibling, or sibling-bound.
A TeamTask that isn't a sibling or sibling-bound is when both the ProjectId and the ParentTeamTaskId are both null.

At creation of a TeamTask, we will automatically set the priority of a sibling or sibling-bound to be 
of the least important, meaning, the number of current siblings plus one. Later, the priority of individual
TeamTasks.

## TeamTaskStatus
TeamTaskStatus describes the state of progress of a TeamTask.

- **Id (int)**. The unique identifier
- **Name (string)**. The name of the TeamTaskStatus. This must be unique.

The possible name values are "active", and "inactive".

## Assignment
An assignment represents the issuing a user to a specific TeamTask. This is actually the link table between a
TeamTask and a User (Assignee), thus multiple users may be assigned to the same TeamTask.

- **Id (int)**. The unique identifier
- **AssignorId (int, required)**. Who created the assignment. Only user with an admin role can create assignments. This property
is set only on creation, and may not be changed.
- **AssigneeId (int, nullable)**. Who the TeamTask is assigned to. It is nullable to allow the creation of this record when
the administrator has not yet decided who will be responsible for a TeamTask.
- **CreatedDate (DateTime, required)**. When the assignment was first saved to data store.
- **Description (string)**. Specific instructions to the Assignee about the task assigned to them.

## User
Since we will use Microsoft's Identity Framework to manage user accounts, we will not be concerned about the properties
of a user. The only relevant information we need about a user is the user Id and whether the user has the admin role.