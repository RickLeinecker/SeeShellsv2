import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Typography, Card } from '@material-ui/core';
import PersonPinIcon from '@material-ui/icons/PersonPin';

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
    text_alt: {
        padding: '1%',
    },
    quote: {
        color: 'white',
        backgroundColor: '#424242',
        padding: '2%',
        maxWidth: '500px',
        maxHeight: '300px',
        minWidth: '500px',
        minHeight: '300px',
        margin: '1%',
        display: 'flex',
        flexDirection: 'column',
        position: 'relative',
    },
    cards: {
        display: 'flex',
        justifyContent: 'space-around',
        flexWrap: 'wrap',
        width: '100%',
        height: '70%',
        alignSelf: 'center',
    },
    icon: {
        width: '75px',
        height: '75px',
        alignSelf: 'flex-end',
        paddingTop: '5%',
        position: 'absolute',
        bottom: '5px',
    },
}

class Parsing extends React.Component {

    render() {
        return (
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>The SeeShells Parser</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Shellbags record folder viewing preferences on the Windows Operating System. 
                    They are stored in user profile registry hives under the HKEY_USERS registry key. 
                    SeeShells extracts shellbag data from the active registry by using the Win32 API 
                    to read registry entries from known shellbag root locations. SeeShells can also 
                    extract shellbags from the following offline registry hive files:
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>UsrClass.dat:</b> A user’s class registry hive, commonly found at C:\Users\[USERNAME]\AppData\Local\Microsoft\Windows\UsrClass.dat
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>NTUSER.dat:</b> A user’s profile registry hive, commonly found at C:\Users\[USERNAME]\NTUSER.dat
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Both of these files are loaded as HKEY_USER stores when loaded on a live Windows system.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Parsing the shellbags is separated into three phases, Registry Key Extraction, Shellbag Field Extraction, and Timestamp Extraction. 
                    <br/>Following these stages is the Event Generation stage, which is part of SeeShells' analysis capabilities.
                </Typography>
                <div className={this.props.classes.cards}>
                    <Card className={this.props.classes.quote}>
                        <Typography variant="button" className={this.props.classes.text_alt}>
                            <b>Quote from the devs:</b>
                        </Typography>
                        <Typography variant="subtitle2" className={this.props.classes.text}>
                            ❝ During the Registry Key Extraction phase, an internal description of the newly added 
                            registry hive is created. We also create a data object that describes the user of that 
                            particular hive. In an effort to try to learn the username anyways, 
                            we search the registry hive for strings containing file system paths that reference 
                            a user’s home directory. The most common home directory found in the hive is almost 
                            always the home directory of the hive owner. From the registry owner’s home directory 
                            path, we can obtain the owner’s username by taking the name of the home folder. ❞
                        </Typography>
                        <PersonPinIcon className={this.props.classes.icon}/>
                    </Card>
                    <Card className={this.props.classes.quote} elevation={3}>
                        <Typography variant="button" className={this.props.classes.text_alt}>
                            <b>Quote from the devs:</b>
                        </Typography>
                        <Typography variant="subtitle2" className={this.props.classes.text}>
                            ❝ During the shellbag field extraction phase, the shellbag byte strings from the 
                            previous phase are parsed into human-readable fields. The parser attempts to assign 
                            types to each shellbag byte array using sets of “signature bits” that are unique to 
                            a given shellbag type. ❞
                        </Typography>
                        <PersonPinIcon className={this.props.classes.icon}/>
                    </Card>
                    <Card className={this.props.classes.quote}>
                        <Typography variant="button" className={this.props.classes.text_alt}>
                            <b>Quote from the devs:</b>
                        </Typography>
                        <Typography variant="subtitle2" className={this.props.classes.text}>
                            ❝ During the timestamp extraction phase, timestamps are extracted from the parsed ShellItems 
                            produced by the previous stage, as the name suggests. The extracted timestamps are converted 
                            to “Intermediate Timeline Events”. 
                            The newly constructed Intermediate Timeline Events are sorted in chronological order before 
                            they are passed to the event generation phase. ❞
                        </Typography>
                        <PersonPinIcon className={this.props.classes.icon}/>
                    </Card>
                </div>
            </div>
        )
    }
}

export default withStyles(styles)(Parsing);