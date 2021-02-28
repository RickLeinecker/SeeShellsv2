import React from 'react';
import beach from '../assets/beach2.png';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';
import Paper from '@material-ui/core/Paper';
import Button from '@material-ui/core/Button';
import ReactPlayer from "react-player";

const styles = {
    downloadPage: {
        display: 'flex',
        flexFlow: 'column wrap',
        justifyContent: 'center',
        height: '100%',
        width: '100%',
    },
    image: {
        display: 'flex',
        width: '100%',
        height: '600px',
        textAlign: 'center',
        justifyContent: 'center',
        alignSelf: 'center',
        backgroundImage: 'url(' + beach + ')',
    },
    downloadContainer: {
        display: 'flex',
        justifyContent: 'center',
        width: '100%',
    },
    contentContainer: {
        display: 'flex',
        justifyContent: 'center',
        flexDirection: 'column',
        backgroundColor: '#424242',
        width: '30%',
        alignItems: 'center',
        color: 'white',
        fontSize: '40px',
        height: '70%',
        position: 'absolute',
        alignSelf: 'center',
        minWidth: '300px',
    },
    title: {
        fontSize: '50px',
        fontWeight: 'bold',
        textAlign: 'center',
    },
    button: {
        display: 'flex',
        backgroundColor: '#33A1FD',
        '&:hover': {
            backgroundColor: '#EF476F',
        },
        color: 'white',
        fontSize: '20px',
        margin: '0px',
        justifyContent: 'center',
        marginTop: '5%',
    },
    video: {
        paddingTop: '5%',
        height: '40%',
        width: '80%',
    },
}

class DownloadPage extends React.Component {
    render() {
        return(
            <Paper className={this.props.classes.downloadPage}>
                <Paper className={this.props.classes.downloadContainer}>
                    <div className={this.props.classes.image}/>
                    <Paper className={this.props.classes.contentContainer}>
                        <p className={this.props.classes.title}>Download SeeShells</p>
                        <Button className={this.props.classes.button}>SEESHELLS.EXE</Button>
                        <div className={this.props.classes.video}>
                            <ReactPlayer width="100%" height="100%" url="https://www.youtube.com/watch?v=IZrd86723Hc"/>
                        </div>
                    </Paper>
                </Paper>
            </Paper>
        );
    }
}

export default withStyles(styles)(withRouter(DownloadPage));