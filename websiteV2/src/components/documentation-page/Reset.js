import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Typography, Paper } from '@material-ui/core';
import ReactPlayer from "react-player";
import Reset from '../../assets/reset.png';
import ConfirmationDialog from '../../assets/confirmation-dialog.png';

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

class Themes extends React.Component {

    render() {
        return (
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>Resetting the Application</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    To clear SeeShells of all hives loaded into it:
                </Typography>
                <Paper className={this.props.classes.video}>
                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=3IOpEqnULB4"/>
                </Paper>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    1. Select the reset button in the top left corner of the application.
                </Typography>
                <img src={Reset} alt="reset-seeshells" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    2. On the diaglog that appears, select yes.
                </Typography>
                <img src={ConfirmationDialog} alt="confirmation-dialog" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    3. SeeShells will reopen, and can be used again.
                </Typography>
            </div>
        )
    }
}

export default withStyles(styles)(Themes);