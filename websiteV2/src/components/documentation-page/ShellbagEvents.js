import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import ShellbagEvent from '../../assets/shellbag-event.png';

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
}

class ShellbagEvents extends React.Component {

    render() {
        return (
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>Shellbag Events</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Shellbags store information that can be used to make inferences about how a Windows machine was used in the past. 
                </Typography>
                <img src={ShellbagEvent} alt="shellbag-event" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    SeeShells analyzes the shellbags stored in a Windows registry hive and interprets them as Shell Events. 
                    Each Shell Event documents an action taken by a user that resulted in a change to their Windows Machine. 
                    For example, when a user connects a removable storage device to their Windows system, the file system is 
                    altered to display the removable storage device and its contents. The resulting changes to the user's 
                    Windows registry hive can be read and interpreted by SeeShells as a Shell Event that describes the Removable 
                    Storage Device Connect action.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Shell Events contain key information about an action taken on a Windows system including the type of 
                    action, the user that performed the action, the time at which the action was taken, any file system 
                    locations associated with the action, and all parsed shellbag data that provide evidence for the action.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Shellbag Events include:
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Item Creation
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Item Last Access
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Item Last Modify
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                     Item Last Registry Write
                 </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Program Installation Event
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Feature Update Event
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    File Download Event 
                </Typography>
            </div>
        )
    }
}

export default withStyles(styles)(ShellbagEvents);