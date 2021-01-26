const app = require('express')();
const port = 8000

app.get('/', (req, res) => {
    res.send("Hello World")  
});

app.listen(port, () => {
    console.log(`listen ${port}`)
});