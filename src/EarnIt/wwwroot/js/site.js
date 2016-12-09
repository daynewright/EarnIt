// Write your Javascript code.


//sample for getting the user and logging in

/*
$.ajax({
    headers: { 
        'Accept': 'application/json',
        'Content-Type': 'application/json' 
    },
	type : 'POST',    
    url: "http://localhost:5000/account/loginjson",
    data: JSON.stringify({"email" : "daynewr@gmail.com", "password" : "Ralston1$"}),
    success: function(data) { console.log('response: ' + data) }
});

$.ajax({
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
    },
    type: 'POST',
    url: "http://localhost:5000/account/registerjson",
    data: JSON.stringify({"email": "daynewr@gmail.com", "password": "Ralston1$", "confirmpassword": "Ralston1$"}),
    success: function(data) { console.log( 'response: ' + data)}
})

$.get('http://localhost:5000/account/getuser', function(data){
   console.log(data.user);
});

*/