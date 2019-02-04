

/* Checks the amount of items added and displaying it to the customers cart */
(function () {

    const uri = "/api/product/getitem";

    // DOM ELEMENTS
    const addButtons = document.querySelectorAll(".add");
    const cartList = document.querySelector("#cartList");


    // Add listeners
    addButtons.forEach(node => {
        node.addEventListener("click", appendData);
    });

    function appendData(node) {
        // Do stuff
    }

    async function getData() {
        let response = await fetch(uri);
        let data = await response.json();
        console.log(data);
    }

    getData();

})();