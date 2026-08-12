[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/nEp1rPdw)
# Assignment: Building a Full-stack Cafe POS System

## Introduction

The 4th Wall Cafe wants to create a point-of-sale application MVC that the staff can use in the browser. The application will utilize all the concepts learned in this module, including LINQ and SQL queries, ORM, ASP.NET Core, and MVC.

### Starter Files

Start a new blank solution for this assignment.

## Requirements

This assignment has the following requirements:

### Views

- **Home Menu**: Display buttons to create an order, view order details on an existing active order, and list open orders (no payment type).

- **Creating an Order**: An order requires a server (ensure the service is active based on the hire date/term date). The order date should be the current day/time.

- **Order Details**: Display the selected order with details, including items in order (this list will be empty on creation), the assigned server, order date, and order total, plus buttons to process payment and add items to order

- **Item Addition**: Allow a server (the user) to select an open order and then add items to an order based on the `ItemID` and `Quantity`.

- **Payment Process**: Query the user for the payment type. We do not store payment information; when a payment type is assigned to an order, we consider it closed. (Assume an external system will process payment and that the order system is not connected to it)

- All views except Home should have a Back Button that returns to the previous page

See the included wireframe for suggested layout. Note, the layout is suggested, and includes notes on additional features to consider. These "considerations" are **not required** but encouraged. You are welcome and encouraged to create a more custom and unique set of views, the wireframes only outline the base level functionality and structure required.

### Application Pages

* The home page of the application lists open order and has a button that allows the user to create a new order.
* The new order page contains a list of active servers to which the user can assign an order. Selecting a server navigates to the order creation form.
* The order creation form, once submitted, creates an order and associates it with the server.
* Each open order item on the homepage has a navigation link to the details of the order.
* The order details page lists all items assigned to the order. It also has two buttons: add an item to order and process payment.
* The add an item to order button lists available items. Clicking on an item adds the item to the order and navigates back to the order details page.
* The process payment button navigates to a list of available payment options. Once an option is selected, a form is opened to finalize the order.

### Sample Interaction

Here are some workflow interactions that should be followed at a high level:

**Creating a New Order**

1. The user clicks the **Create New Order** menu item.
2. The system displays active servers.
3. The user selects the server.
4. The system creates a new order and navigates to its details page by invoking the `RedirectToAction()` method with the order id.

**Viewing Open Orders**

1. The user navigates to the homepage.
2. The system displays the open orders (`CafeOrder` items where `PaymentTypeID` is NULL).
3. The user selects the order.
4. The system displays the list of the items added to the order.

**Adding Items to An Order**

1. The user is on the details page of a specific order.
2. The user clicks the **Add Items to Order** button.
3. The system displays the available items (`Item` entity).
4. The user selects the item. Clicking on the item add the item to the order and redirects to the order details page by invoking the `RedirectToAction()` method with the order id.

**Process Payment**

1. The user clicks the **Process Payment** button on the order details page.
2. The system navigates to a page that displays all available payment types.
3. Selecting a payment type opens a form.
4. Submitting the form adds the payment type id to the order, so it's no longer considered to be open.
5. The system navigates to the homepage.

**Hint**

If we need to pass an item identifier through a flow involving multiple pages, we can add it to a custom dynamic property in the `ViewBag` object. For example, if we need to pass on the value of the `orderId` parameter, we can do the following in the controller:

```csharp
ViewBag.OrderId = orderId;
```

The corresponding view will be able to read the `OrderId` property from the `ViewBag` object.

## Deliverables

The deliverable of this project will be a fully functioning Cafe POS application that adheres to the above requirements.

## Conclusion

This project was designed to allow the students to practice all the concepts that were realistically learned in the module. The project closely mimics how real-life enterprise applications are built. Therefore, we gain foundational knowledge of designing and building applications according to specifications by completing this project.
