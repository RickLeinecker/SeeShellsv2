import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Typography, Paper } from '@material-ui/core';
import ReactPlayer from "react-player";
import OnlineParse from '../../assets/online-parse.png';
import ImportSelect from '../../assets/import-select.png';
import OnlineHive from '../../assets/live-registry-parse.png';

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

class OnlineParsing extends React.Component {
    
render() {
        return(
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>Online Parsing</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Online Parsing refers to parsing shellbag data directly from the machine presently in use.
                </Typography>
                <img src={OnlineParse} alt="online-parse" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>To parse an active registry:</b>
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    1. Select the option that says "From Active Registry" from the start menu of the application.
                </Typography>
                <Paper className={this.props.classes.video}>
                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=jxLqeHEcFWo"/>
                </Paper>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    a. Alternatively, if SeeShells is already running, select Import > From Live Registry
                </Typography>
                <img src={ImportSelect} alt="import-select" />
                <Paper className={this.props.classes.video}>
                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=9p-ahxUJ6wI"/>
                </Paper>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    2. The shellbags from the live machine will populate the multiple views available in SeeShells and 
                    the user can explore the shellbags and the extrapolated shellbag events.
                </Typography>
                <img src={OnlineHive} alt="offline-hive" />
            </div>
            
        )
    }
}

export default withStyles(styles)(OnlineParsing);