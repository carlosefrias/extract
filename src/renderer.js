//const electron = require('electron')
import electron from 'electron';
const ipc = electron.ipcRenderer

document.getElementById('zip').addEventListener('click', _=>{
    ipc.send('zip')
})

ipc.on('zip', (evt, path)=>{
    console.log('here...')
})