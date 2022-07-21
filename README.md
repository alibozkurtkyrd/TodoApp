# TodoApp

it's a ASP.NET web api project with ASP.NET Core


---

+ There is a one to many relations between User and Todo tables.

+ Repository Design Pattern is used.

+ Code First Approach is used.

+ "UserId" is foreign. It's set the NULL rather than "CASCADE".

+ Email address set as unique.

+ Due to security reason, passwords were stored in database via hash function.

+ When listing products, they are sorted first by the "IsCompleted" column, then by the "Deadline" column.

+ Data Transfer Object (DTO) was used to prevent unnecessary lines such as password and id in the database to be directly printed on the screen.


+ You can use the "TodoApp.postman_collection.json" file in the main directory to test the code.


---
###  Technologies

* .Net 6
* Entity Framework Core
* Postman
* Sqlite
* Visual Studio Code
* c#

---

[Click](https://drive.google.com/file/d/1icqcwAlePnXsYQu8RHt_kxYluyrITgmd/view?usp=sharing) for a video on how the program works
