import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Typography, Paper } from '@material-ui/core';
import ReactPlayer from "react-player";
import LightTheme from '../../assets/light-theme.png';
import Toggle from '../../assets/toggle.png';

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
                <Typography variant="title" className={this.props.classes.title}>Themes</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    SeeShells has both light and dark themes. It is set to dark by default.
                </Typography>
                <img src={LightTheme} alt="light-theme" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>To switch themes:</b>
                </Typography>
                <Paper className={this.props.classes.video}>
                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=eABrO823f-g"/>
                </Paper>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    1. Click on the toggle in the upper right of the SeeShells Window to switch to light theme.
                </Typography>
                <img src={Toggle} alt="seeshells-toggle" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    2. Click on the toggle again to switch back to dark.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    3. All views will update accordingly with the theme's color scheme.
                </Typography>
            </div>
        )
    }
}

export default withStyles(styles)(Themes);