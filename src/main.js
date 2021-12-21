const {app, BrowserWindow} = require('electron')
const path = require('path')

const createWindow = () =>{
    const win = new BrowserWindow({
        width: 400,
        height: 200
    })

    win.loadFile('./src/index.html')
}

app.whenReady().then(()=>{
    createWindow()

    app.on('activate', _=>{
        if(BrowserWindow.getAllWindows().length === 0) 
            createWindow()
    })
})

app.on('window-all-closed', _=>{
    if(process.platform !== 'darwin')
        app.quit()
})