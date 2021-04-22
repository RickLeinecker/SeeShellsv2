import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Typography, Paper } from '@material-ui/core';
import ReactPlayer from "react-player";
import RegistryImage from '../../assets/live-registry.png';
import RegistryToInspector from '../../assets/registry-to-inspector.png';

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

class RegistryView extends React.Component {

    render() {
        return (
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>Registry View</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The registry view presents all of the data retrieved by SeeShells in a hierarchical view. 
                    This view allows the user to explore the file system for any folders of interest, and use that folder information
                    to narrow down their search for incriminating evidence.
                </Typography>
                <img src={RegistryImage} alt="registry" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>To use the registry view:</b>
                </Typography>
                <Paper className={this.props.classes.video}>
                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=_uB9aRtAUAY"/>
                </Paper>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    1. Clicking the triangle next to a drive or folder name will open up the subtree of folders within it.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    2. Selecting a drive or folder will bring up associated shellbag information in the shell inspector.
                </Typography>
                <img src={RegistryToInspector} alt="registry-to-inspector" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    3. Information presented in the shell inspector can narrow down a timespan of interest.
                </Typography>
            </div>
        )
    }
}

export default withStyles(styles)(RegistryView);