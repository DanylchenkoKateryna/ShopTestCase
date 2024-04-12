const orderList=document.querySelector('.orders-list');
const getAllOrderButton = document.getElementById('getAll-order');
const searchOrderButton = document.getElementById('searchOrder-value');
const addOrderForm=document.querySelector('.add-order-form');
const customerNameValue=document.getElementById('customerName-value');
const customerPhoneValue=document.getElementById('customerPhone-value');
const productIdValue = document.getElementsByClassName('form-control role');
const amountValue = document.getElementsByClassName('form-control role1');
const urlOrder='https://localhost:7118/Order';

function addOrderProduct() {
    const orderProductsContainer = document.getElementById('orderProducts');
    const orderProductTemplate = `
    <div class="orderProduct bg-light m-4">
        <div class="order-product ">
            <label for="productId">Product Id:</label>
            <input type="text" class="form-control role"  id="productId-value" name="orderProducts[${orderProductsContainer.childElementCount}].productId" required>
            <label for="amount">Amount:</label>
            <input type="number" class="form-control role1"  id="amount-value" name="orderProducts[${orderProductsContainer.childElementCount}].amount" required>
        </div>
    </div>
    `;
    orderProductsContainer.insertAdjacentHTML('beforeend', orderProductTemplate);
}

const renderOrders= (orders)=> {
    let output= '';
    orders.forEach(order => {
        output += `
            <div class="card mt-4 col-md-6 bg-light">
                <div class="card-body" data-id=${order.id}>
                    <h5 class="card-title">Id: ${order.id}</h5>
                    <p class="card-text">Date: ${order.createdOn}</p>
                    <p class="card-text">Customer Full Name: ${order.customerFullName}</p>
                    <p class="card-text">Customer Phone:${order.customerPhone}</p>
                    <h6>Products:</h6>
        `;
        // Render products
        order.products.forEach(product => {
            output += `
                <hr>
                <div>
                    <p class="card-text">Product Id: ${product.id}</p>
                    <p class="card-text">Product Name: ${product.name}</p>
                </div>
                <hr>
            `;
        });
        output += `
                </div>
            </div>
        `;
    });
    orderList.innerHTML=output;
}

//Get -read orders
//Method: Get
getAllOrderButton.addEventListener('click', (e) => {
    fetch(urlOrder)
    .then(res=>res.json())
    .then(data=>renderOrders(data))
    
});

//Get - GetById 
//Method: Get
searchOrderButton.addEventListener('click', (e) => {
    e.preventDefault();
    const orderId = document.getElementById('search-order-value').value;
    
    fetch(`${urlOrder}/${orderId}`)
    .then(res => res.json())
    .then(order => {
        // Clear previous search results
        orderList.innerHTML = '';
        renderOrders([order]);
    })
    });


//Create - Insert new order
//Method: POST
addOrderForm.addEventListener('submit',(e)=>{
    e.preventDefault();
    const orderProducts = [];

    for (let i = 0; i < productIdValue.length; i++) {
        const productId = productIdValue[i].value;
        const amount = amountValue[i].value;
        orderProducts.push({ ProductId: productId, Amount: amount });
    }
    fetch(urlOrder,{
        method: 'POST',
        headers: {
            'Content-Type':'application/json'
        },
        body: JSON.stringify({
            CustomerFullName: customerNameValue.value,
            CustomerPhone: customerPhoneValue.value,
            OrderProducts: orderProducts
        })
    })
    .then(res =>res.json())
    .then(data => {
        const dataArray=[];
        dataArray.push(data);
        renderOrders(dataArray);
    })

})