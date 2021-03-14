import React from 'react';
import oldLogo from '../assets/oldLogo.png';
import beach from '../assets/beach2.png';
import '../assets/animation.css';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';
import Paper from '@material-ui/core/Paper';
import Button from '@material-ui/core/Button';
import Typography from '@material-ui/core/Typography';
import Grow from '@material-ui/core/Grow';
import ReactPlayer from "react-player";

const styles = {
    downloadPage: {
        display: 'flex',
        flexFlow: 'column wrap',
        justifyContent: 'center',
        height: '100%',
        width: '100%',
        overflow: 'auto',
    },
    image: {
        display: 'grid',
        width: '100%',
        height: '600px',
        textAlign: 'center',
        alignSelf: 'center',
        backgroundImage: 'url(' + beach + ')',
        overflow: 'hidden',
    },
    logo: {
        height: '100px',
        width: '100px',
        display: 'flex',
        float: 'left',
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
        minWidth: '360px',
        justifySelf: 'center',
    },
    title: {
        fontSize: '50px',
        fontWeight: 'bold',
        textAlign: 'center',
        marginBottom: '5%',
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
                        <div className={this.props.classes.image}>
                            <div id='stars'/>
                            <div id='stars2'/>
                            <div id='stars3'/>
                            <Grow in={true}>
                                <Paper className={this.props.classes.contentContainer}>
                                    <Typography variant="title" className={this.props.classes.title}>Download SeeShells</Typography>
                                    <img src={oldLogo} alt='SeeShells Logo' className={this.props.classes.logo}/>
                                    <Button className={this.props.classes.button} href="https://github.com/RickLeinecker/SeeShells/releases/latest/download/SeeShells.exe">SEESHELLS.EXE</Button>
                                    <div className={this.props.classes.video}>
                                        <ReactPlayer width="100%" height="100%" url="https://www.youtube.com/watch?v=IZrd86723Hc"/>
                                    </div>
                                </Paper>
                            </Grow>

                        </div>
                    </Paper>
                </Paper>
        );
    }
}

export default withStyles(styles)(withRouter(DownloadPage));