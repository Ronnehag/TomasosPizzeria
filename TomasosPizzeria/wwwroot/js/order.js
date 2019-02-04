

/* Checks the amount of items added and displaying it to the customers cart */
(function () {

    const uri = "/api/products/1";

    // DOM ELEMENTS
    const addButtons = document.querySelectorAll(".add");
    const cartList = document.getElementById("cartList");


    // Add listeners
    addButtons.forEach(node => {
        node.addEventListener("click", appendData);
    });

    function appendData() {
        let li = document.createElement("li");
        li.innerHTML = "Hello";
        cartList.appendChild(li);

    }



    async function getData() {
        let response = await fetch(uri);
        let data = await response.json();
        console.log(data);
    }

    getData();

})();