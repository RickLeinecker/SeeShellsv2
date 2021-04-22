import React from 'react';
import logo from '../assets/seeshellsLogo-650.png';
import beach from '../assets/beach3.png';
import github from '../assets/github_PNG15.png';
import '../assets/animation.css';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';
import { Paper, Button, Typography, Grow } from '@material-ui/core';

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
        display: 'flex',
        width: '100%',
        height: '600px',
        textAlign: 'center',
        backgroundImage: 'url(' + beach + ')',
        overflow: 'hidden',
        borderRadius: '0',
    },
    logo: {
        height: '150px',
        width: '150px',
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
    githubContainer: {
        display: 'flex',
        justifyContent: 'center',
    },
    github: {
        paddingTop: '5%',
        width: '45%',
    },
}

/*
*   DownloadPage.js
*   - serves the link to the SeeShells executable, pulled directly from github releases to the SeeShells repository
*/
// TODO: fix the download page on chrome/edge
class DownloadPage extends React.Component {
    render() {
        return(
            <Paper elevation={0} className={this.props.classes.downloadPage}>
                <Paper elevation={0} className={this.props.classes.downloadContainer}>
                    <Paper elevation={0} className={this.props.classes.image}>
                        <div id='stars'/>
                        <div id='stars2'/>
                        <div id='stars3'/>
                    </Paper>
                    <Grow in={true}>
                        <Paper className={this.props.classes.contentContainer}>
                            <Typography variant="title" className={this.props.classes.title}>Download SeeShells</Typography>
                            <img src={logo} alt='SeeShells Logo' className={this.props.classes.logo}/>
                            <Button className={this.props.classes.button} href="https://github.com/ShellBags/v2/releases/download/v2.0-beta.3/SeeShellsV2.zip">SEESHELLS.EXE</Button>
                            <a href="https://github.com/ShellBags/v2" className={this.props.classes.githubContainer} >
                                <img src={github} alt='Github Logo' className={this.props.classes.github}/>
                            </a>
                        </Paper>
                    </Grow>
                </Paper>
            </Paper>
        );
    }
}

export default withStyles(styles)(withRouter(DownloadPage));