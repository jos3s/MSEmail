let loginForm = document.getElementById("loginButton");

console.log("aaa")

loginForm.addEventListener("click", function (e) {
    console.log("aaa")
    e.preventDefault()

    var login = document.getElementById("login").value;
    var password = document.getElementById("password").value;

    var _data = {
        "email": login,
        "password": password
    }

    fetch('https://localhost:7281/login',
            {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(_data)
            })
        .then(response => response.json())
        .then(data => {
            localStorage.setItem("token", data.token)
            window.location.href = "https://localhost:7281/home/";
        })
        .catch(error => console.log(error));
})