const {app, BrowserWindow, ipcMain} = require('electron')
const path = require('path')
const ipc = ipcMain
const zip = require('./zip.js')

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

ipc.on('zip', _=>{
    zip(path=>{
        return "test..."
    })
})