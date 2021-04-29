import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Typography, Paper } from '@material-ui/core';
import ReactPlayer from "react-player";
import ShellInspector from '../../assets/shell-inspector.png';

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
                <Typography variant="title" className={this.props.classes.title}>Shell Inspector</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The shell inspector displays the raw shellbag data fields. 
                </Typography>
                <img src={ShellInspector} alt="shellbag-inspector" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The data fields most common to all shellbag types are displayed at the top, while more specific and varied data is contained within
                    the Shellbag Information and Shellbag Fields. Shellbags can have different fields depending on shellbag types.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    After selecting a shellbag from one of the other views, the user can use this display to look at the data fields.
                </Typography>
                <Paper className={this.props.classes.video}>
                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=y2vmbEk9Jxw"/>
                </Paper>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    This information is extracted from an offset in the hex values, visible in the Hex Viewer.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    It is possible for multiple shellbags to appear in the shell inspector. When viewing a shellbag event, all shellbags 
                    related to that event will appear in this window, and the user can scroll through them. During a Windows Update, many 
                    of these events will occur within a minute of each other, and will all appear in the inspector.
                </Typography>
            </div>
            
        )
    }
}

export default withStyles(styles)(OnlineParsing);