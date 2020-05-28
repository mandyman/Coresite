# Coresite

WebSite for a Mexican restaurant, made in asp.net core, with MVC arch and Razor pages.
It has several users  types:

- Customer.
- Manager.
- Kitchen.
- FrontDesk.

Roles of every user type:

- Customer is the basic user type, it can only access the main page and the menus details display there, this only after registration.
- Manager is the main role in the database. It can create accounts for all types of users, but mainly other managers users and other  employs, like Kitchen (employs assign to the kitchen area), and FrontDesk. Also is the one that can create new Categories and SubCategories for the dishes, and can create menu items that correspond with this categories and subcategories.
- Kitchen is the user type that can create menus and dishes but no new Categories or Subcategories, and in the current state of the project behaves basically as a user that can create dishes.
- FrontDesk is an employee that in the current state of the project behaves as a Customer user.

The website has a shopping cart but the buy button doesn’t do anything at this moment, it should redirect to a pay platform to pay the amount of the items in the shopping cart.  
The ContentManagement drop list only appears to the Management users and Kitchen users, the first with 4 links to Category, Subcategory, MenuItem and User; and the Kitchen user types only sees MenuItem.
Main Views and functionalities (Every one of this view has 3 function that can be apply to every item, Edit, Details and Delete ):

- Category: It shows the list of all the Categories created; also it can add a new Category.
- SubCategory: It shows the list of all the SubCategory created; also it can add a new SubCategory, when a manager creates a SubCategory it has to be unique for every Category. 
- MenuItem: It shows the list of all the MenuItem created; also it can add a new MenuItem, a new MenuItem has to have a Category and a SubCategory.
- User: It shows the list of the Users and can Create more users; also in this view accounts can be lock and unlock.
- Registration: To register the current user.
- Login: To grant access to a created account.
- Home View: It has the full menu with filters for every Category. Every item has a Detail button that links to DetailsAddToCart view.
- DetailsAddToCart: It can only be access by login users and it shows the details of the item, and it can chose amount to buy, and can also a button to add to the shopping cart.

The Database at the start only has one user name admin of type Manager with email: "admin@gmail.com" and password: "Admin123."
