const productList=document.querySelector('.products-list');
const addProductForm=document.querySelector('.add-product-form');
const codeValue=document.getElementById('code-value');
const nameValue=document.getElementById('name-value');
const priceValue=document.getElementById('price-value');
const btnSubmit=document.querySelector('.btn');
const searchButton = document.getElementById('search-product');
const getAllButton = document.getElementById('getAll-product');
const url='https://localhost:7118/Product';


const renderProducts= (products)=> {
    let output= '';
    products.forEach(product => {
        output +=`
            <div class="card mt-4 col-md-6 bg-light">
                <div class="card-body" data-id=${product.id}>
                <h5 class="card-title">${product.name}</h5>
                <h6>Id:</h6>
                <p class="card-text">${product.id}</p>
                <h6>Code:</h6>
                <p class="card-text1">${product.code}</p>
                <h6>Price:</h6>
                <p class="card-text2">${product.price}</p>
                <a href="#" class="card-link" id="edit-product">Edit</a>
                </div>
            </div>`;
    });
    productList.innerHTML=output;
}

//Get -read products
//Method: Get
getAllButton.addEventListener('click', (e) => {
        fetch(url)
        .then(res=>res.json())
        .then(data=>renderProducts(data))
    });



function getProductById(productId) {
    // Fetch the product data by ID
    return fetch(`${url}/${productId}`)
        .then(res => res.json())
        
        .then(product => {
            // Clear previous search results
            productList.innerHTML = '';

            // Render the found product
            renderProducts([product]);
        })
}


function getProductByCode(productCode) {
    return  fetch(`${url}/${productCode}/getByCode`)
        .then(res => res.json())
        
        .then(product => {
            // Clear previous search results
            productList.innerHTML = '';

            // Render the found product
            renderProducts([product]);
        })
}
//Get - GetById or code
//Method: Get
searchButton.addEventListener('click', (e) => {
    e.preventDefault();
    // Get the product ID from the input field
    const searchValue  = document.getElementById('search-value').value;
    console.log(searchValue );
    if (!isNaN(searchValue)) {
        getProductById(searchValue)
    } else {
        getProductByCode(searchValue)
    }
    });


productList.addEventListener('click',(e)=>{
    e.preventDefault();
    let editButtonIspressed = e.target.id == 'edit-product';

    let id=e.target.parentElement.dataset.id;

    if(editButtonIspressed){
        const parent =e.target.parentElement;
        let nameContent=parent.querySelector('.card-title').textContent;
        let codeContent=parent.querySelector('.card-text1').textContent;
        let priceContent=parent.querySelector('.card-text2').textContent;

        codeValue.value = codeContent;
        nameValue.value= nameContent;
        priceValue.value=priceContent;
    }

    //Update -update the existing product
    //Method: PUT
    btnSubmit.addEventListener('click',(e)=>{
        e.preventDefault();
        fetch(`${url}/${id}`,{
            method: 'PUT',
            headers:{
                'Content-Type':'application/json'
            },
            body:JSON.stringify({
                code:codeValue.value,
                name:nameValue.value,
                price:priceValue.value
            })
        })
         .then(()=> location.reload())
    })
})




//Create - Insert new product
//Method: POST
addProductForm.addEventListener('submit',(e)=>{
    e.preventDefault();

    fetch(url,{
        method: 'POST',
        headers: {
            'Content-Type':'application/json'
        },
        body: JSON.stringify({
            code:codeValue.value,
            name:nameValue.value,
            price:priceValue.value

        })
    })
    .then(res =>res.json())
    .then(data => {
        const dataArray=[];
        dataArray.push(data);
        renderProducts(dataArray);
    })

    //reset input field to empty
    codeValue.value='';
    nameValue.value='';
    priceValue.value='';
})
