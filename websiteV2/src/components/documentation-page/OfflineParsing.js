import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Typography, Paper } from '@material-ui/core';
import ReactPlayer from "react-player";
import OfflineParse from '../../assets/offline-parse.png';
import ImportSelect from '../../assets/import-select.png';
import RegistryHive from '../../assets/registry-hive.png';
import ImportHive from '../../assets/import-hive.png';
import OfflineHive from '../../assets/offline-hive.png';

const styles = {
    content: {
        height: '100%',
        width: '100%',
        display: 'flex',
        justifyContent: 'flex-start',
        flexDirection: 'column',
        alignItems: 'center',
        overflow: 'auto',
        borderRadius: '0',
    },
    title: {
        fontSize: '50px',
        fontWeight: 'bold',
        marginTop: '1%',
        alignSelf: 'center',
        color: '#33A1FD',
        textAlign: 'center',
    },
    text: {
        textAlign: 'center',
        padding: '1%',
    },
    video: {
        maxHeight: '360px',
        maxWidth: '640px',
        minHeight: '360px',
        minWidth: '300px',
        height: '50%',
        width: '50%',
        margin: '10px',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        backgroundColor: '#424242',
    },
}

class OfflineParsing extends React.Component {

    render() {
        return (
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>Offline Parsing</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Offline Parsing refers to parsing shellbag data from a Windows Registry hive, the UsrClass.dat file.
                </Typography>
                <img src={OfflineParse} alt="offline-parse" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>To get an offline hive for SeeShells from a Windows machine:</b>
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    1. Access the Windows File Explorer as an Admin user
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    2. Navigate to $USERHOME\AppData\Local\Microsoft\Windows\
                </Typography>
                <img src={RegistryHive} alt="registry-hive" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    3. Copy and Paste the UsrClass.dat file from the Windows folder to the destination folder
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Currently, SeeShells only supports the parsing of .dat files. UsrClass.dat is the 
                    recommended file to be parsed in SeeShells since it contains information pertaining to 
                    shellbags. Conversely, NTUSER.dat is a supported filetype, but usually contains no information 
                    pertaining to shellbags and will not be useful to a SeeShells user.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>To load a registry hive into SeeShells:</b>
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    1. Run SeeShells
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    2. Under "Import Registry" on the Start Menu, select "From Registry File"
                </Typography>
                <Paper className={this.props.classes.video}>
                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=O6HnfsjFQAg"/>
                </Paper>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    a. Alternatively, a user can navigate to the global menu in the upper left corner and select
                    File > Import > From Offline Registry
                </Typography>
                <img src={ImportSelect} alt="import-select" />
                <Paper className={this.props.classes.video}>
                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=ngPgepgKYek"/>
                </Paper>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    3. In the File Dialog, navigate to the UsrClass.dat file to be parsed
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    4. Click on the UsrClass.dat file and select Open
                </Typography>
                <img src={ImportHive} alt="import-hive" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    5. The Timeline, Registry, Shellbags, and Events views will populate with the parsed file contents
                </Typography>
                <img src={OfflineHive} alt="offline-hive" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Once the UsrClass.dat file is loaded into SeeShells, the user will be able to explore the 
                    shellbags and extrapolated shellbag events using the multiple views available in SeeShells.
                </Typography>
            </div>
        )
    }
}

export default withStyles(styles)(OfflineParsing);