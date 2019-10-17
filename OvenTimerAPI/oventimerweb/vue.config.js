module.exports = {    
    devServer: {
        // CORS Hack. This allows a work-around for CORS by redirecting all service calls at localhost:8080 to the proxy URL  
        // proxy: 'http://localhost:5000/'
    },
    filenameHashing: false,
    outputDir: "../wwwroot/"  
}