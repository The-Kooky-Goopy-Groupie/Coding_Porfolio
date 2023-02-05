// function most basic
function speak() {
    console.log('hello');
    console.log('hola');
    console.log('namaste');
}
speak();

// Use function to transform data  - capitalize string
function capitalfy(content) {
    // if it's not a string, return an error message
    if (typeof content !== "string") {
        console.error("This is not a string");
        return;
    }
    //otherise, capitalize the first letters of the word
    content = content.replace(/^(\w)(.+)/, (match, p1, p2) => p1.toUpperCase() + p2.toLowerCase());
    return content;
}
// capitalfy("here is my text.");
//or
var storit = capitalfy("here is my text.");
storit;

//function expression isEven
function isEven(num) {
    if (num % 2 === 0) {
        return true;
    } else {
        return false;
    }
}
isEven(88);

//anonymous function
var isEven1 = function (num) {
    if (num % 2 === 0) {
        return true;
    } else {
        return false;
    }
}
isEven1(88);

//arrow functions
isArrow = a => {
    if (a % 2 === 0) {
        return true;
    } else {
        return false;
    }
}
isArrow(50);

//arrow expression
isArrow1 = num => (num % 2);
isArrow1(46);

//callback function
function doubleIt(num) {
    return (num *= 2);
}
var myNumArray = [2,4,6,8,10];
var myDouble = myNumArray.map(doubleIt);
console.log(myNumArray);
console.log(myDouble);

//Array forEach()
myNumArray.forEach(function(num) {
    console.log("My array contains", num);
});

//Array forEach() | for() vs forEach
const paintings = [
    {title: "Girl with a Pearl Earring", artist: "Vermeer", year: 1665},
    {title: "Artist Holding a Thistle", artist: "Durer", year: 1493},
    {title: "Wheatfield with Crows", artist: "Van Gogh", year: 1890},
    {title: "Burial at Ornans", artist: "Courbet", year: 1849},
    {title: "Sunflowers", artist: "Van Gogh", year: 1889}
];
// version 1
for (let i=0; i<paintings.length; i++) {
    console.log(paintings[i].title + ' by ' + paintings[i].artist);
}
// version 2 –  uses function and arrow syntax
paintings.forEach( (p) => {
    console.log(p.title + ' by ' + p.artist)
});

//Array find()
const courbet = paintings.find( p => p.artist === 'Courbet' );
console.log(courbet.title); // Burial at Ornans

//Array filter
// vangoghs will be an array containing two painting objects
const vangoghs = paintings.filter(p => p.artist === 'Van Gogh');
vangoghs.forEach ( (p) => {
    console.log(p);
});

//Array filter regular expression
const re = new RegExp('with', 'i'); // case insensitive
const withs = paintings.filter( p => p.title.match(re) );
withs.forEach ( (p) => {
    console.log(p);
});

//array map
const options2 = paintings.map( p => `<li>${p.title} (${p.artist})</li>` );
console.log(options2);


//Fetching Data from a Web API
let osType = fetch("http://www.randyconnolly.com/funwebdev/services/visits/os.php?name=Windows");

//setTimeOut example of Async Request
window.setTimeout(()=>{console.log("waiting")},3000);
console.log("display this first");

//Fetching Data - order of return
console.log('before the fetch()');
let prom = fetch('https://www.randyconnolly.com/funwebdev/services/visits/os.php?name=id');
prom.then( function(response) {
    // do something with this data
    console.warn('response received!!!');
});
console.log('after the then()');

//will this output json data or another promise object?
let data = prom.then( function(response) {
    return response.json();
});
console.log(data);

//better response
let osType = fetch("https://www.randyconnolly.com/funwebdev/services/visits/os.php?name=id");
let data = osType.then( function(response) {
    return response.json();
});
data.then( function (data) {
    // now finally do something with the JSON data
    console.log(data);
});

// promisified version of the transfer task
function transferToCloud(filename) {
    return new Promise( (resolve, reject) => {
        // just have a made-up AWS url for now
        let cloudURL =
            "http://bucket.s3-aws-region.amazonaws.com/makebelieve.jpg";
        // if passed filename exists then upload ...
        if ( existsOnServer(filename) ) {
            performTransfer(filename, cloudURL);
            resolve(cloudURL);
        } else {
            reject( new Error('filename does not exist'));
        }
    });
}
// use this function
transferToCloud(file)
    .then( url => extractTags(url) )
    .then( url => compressImage(url) )
    .catch( err => logThisError(err) );

//web speech browser api
const utterance = new SpeechSynthesisUtterance('Hello world');
speechSynthesis.speak(utterance);

//GeoLocation browser API
let voices = speechSynthesis.getVoices();
let utterance = new SpeechSynthesisUtterance('Hello world');
utterance.voice = voices[3];
utterance.rate = 1.5;
utterance.pitch = 1.3;
speechSynthesis.speak(utterance);

if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(haveLocation,geoError);
} else {
    cosnsole.log("geolocation not supported or accepted");
}
function haveLocation(position) {
    const latitude = position.coords.latitude;
    const longitude = position.coords.longitude;
    const altitude = position.coords.altitude;
    const accuracy = position.coords.accuracy;
    // now do something with this information
    console.log(latitude,longitude,altitude,accuracy);
}
function geoError(error) {   }