import React from 'react';
import beach from '../assets/beach2.png';
import pearl from '../assets/pearl.png';
import '../assets/animation.css';
import { withStyles } from '@material-ui/core/styles';
import { withRouter, HashRouter as Router } from 'react-router-dom';
import { Button, Paper, Typography, Grow, Tooltip } from '@material-ui/core';

const styles = {
    frontPage: {
        height: '100%',
        width: '100%',
        overflow: 'auto',
        borderRadius: '0',
    },
    barContent: {
        height: '100%',
        display: 'flex',
        flexFlow: 'column',
        justifyContent: 'space-between',
        alignItems: 'center',
        borderRadius: '0',
    },
    intro: {
        color: '#F2F2F2',
        fontSize: 'calc(12px + 6vw)',
        margin: '0px',
        alignSelf: 'flex-start',
        paddingLeft: '10%',
    },
    description: {
        color: '#F2F2F2',
        fontSize: 'calc(12px + 2vw)',
    },
    image: {
        display: 'grid',
        width: '100%',
        height: '40%',
        maxHeight: '350px',
        minHeight: '200px',
        textAlign: 'center',
        backgroundImage: 'url(' + beach + ')',
        overflow: 'hidden',
        borderRadius: '0',
    },
    downloadButton: {
        display: 'flex',
        backgroundColor: '#33A1FD',
        '&:hover': {
            backgroundColor: '#EF476F',
        },
        color: 'white',
        fontSize: '2vh',
        marginBottom: '1%',
        marginTop: '1%',
    },
    descriptionAlt: {
        color: '#33A1FD',
        fontWeight: 'bold',
        display: 'contents',
    },
    title: {
        color: '#082998',
        fontSize: 'calc(12px + 2.5vw)',
        fontWeight: 'bold',
    },
    featuresContainer: {
        height: '60%',
        display: 'flex',
        justifyContent: 'space-evenly',
        alignItems: 'center',
        flexDirection: 'column',
    },
    featuresButton: {
        backgroundColor: '#33A1FD',
        '&:hover': {
            backgroundColor: '#EF476F',
        },
        color: 'white',
        margin: '0px',
        fontSize: '2vh',
    },
    infoContainer: {
        display: 'flex',
        justifyContent: 'center',
        flexWrap: 'wrap',
        width: '100%',
        height: '70%',
        alignSelf: 'center',
    },
    info: {
        display: 'flex',
        alignItems: 'center',
        width: '35%',
        height: '30%',
    },
    text: {
        fontSize: 'calc(12px + 0.5vw)',
    },
    pearl: {
        height: '2.5vw',
        width: '2.5vw',
        verticalAlign: 'center',
        marginRight: '10px',
    },
};

/*
*   FrontPage.js
*   - the front page of the website
*/
class FrontPage extends React.Component {
    constructor(props) {
        super(props);

        this.handleClick = this.handleClick.bind(this);
    }

    /*
    *   handleClick(event)
    *   - takes in a react event object on every button click
    *   - redirects the user to the page associated with the button's id
    */
    handleClick(event) {
        this.props.history.push("/" + event.currentTarget.id);
    }

    render() {
        return(
            <Router basename="/">
                <Paper className={this.props.classes.frontPage}>
                    <Paper elevation={0} className={this.props.classes.image}>
                        <div id='stars'/>
                        <div id='stars2'/>
                        <div id='stars3'/>
                        <Grow in={true}>
                            <div className={this.props.classes.barContent}>
                                <Typography variant="title" className={this.props.classes.intro}>SEESHELLS</Typography>
                                <Typography variant="title" className={this.props.classes.description}>
                                    A <span className={this.props.classes.descriptionAlt}>digital forensics tool</span> for analyzing Windows Registry Artifacts.
                                </Typography>
                                <Button className={this.props.classes.downloadButton} onClick={this.handleClick} id="download">GET THE TOOL</Button>
                            </div>
                        </Grow>
                    </Paper>
                    <Paper elevation={0} className={this.props.classes.featuresContainer}>
                        <Tooltip title="We search seeshells on your C: shore!">
                            <Typography variant="title" className={this.props.classes.title}>Why SeeShells?</Typography>
                        </Tooltip>
                        <Grow in={true}>
                            <Paper elevation={0} className={this.props.classes.infoContainer}>
                                <Paper elevation={0} className={this.props.classes.info}>
                                    <img src={pearl} alt='pearl' className={this.props.classes.pearl}/>
                                    <Typography className={this.props.classes.text}><span className={this.props.classes.descriptionAlt}>Quick parsing</span> of Windows Registry artifacts</Typography>
                                </Paper>
                                <Paper elevation={0} className={this.props.classes.info}>
                                    <img src={pearl} alt='pearl' className={this.props.classes.pearl}/>
                                    <Typography className={this.props.classes.text}><span className={this.props.classes.descriptionAlt}>Analysis of artifacts</span> with event-based pattern recognition</Typography>
                                </Paper>
                                <Paper elevation={0} className={this.props.classes.info}>
                                    <img src={pearl} alt='pearl' className={this.props.classes.pearl}/>
                                    <Typography className={this.props.classes.text}><span className={this.props.classes.descriptionAlt}>Timeline View</span> for a holistic view of computer activity</Typography>
                                </Paper>
                                <Paper elevation={0} className={this.props.classes.info}>
                                    <img src={pearl} alt='pearl' className={this.props.classes.pearl}/>
                                    <Typography className={this.props.classes.text}><span className={this.props.classes.descriptionAlt}>Filtering</span> for specific activities and trends</Typography>
                                </Paper>
                                <Paper elevation={0} className={this.props.classes.info}>
                                    <img src={pearl} alt='pearl' className={this.props.classes.pearl}/>
                                    <Typography className={this.props.classes.text}><span className={this.props.classes.descriptionAlt}>Exportation</span> for convenient reporting</Typography>
                                </Paper>
                                <Paper elevation={0} className={this.props.classes.info}>
                                    <img src={pearl} alt='pearl' className={this.props.classes.pearl}/>
                                    <Typography className={this.props.classes.text}><span className={this.props.classes.descriptionAlt}>Hierarchical view</span> of shell items to find folders of interest</Typography>
                                </Paper>
                            </Paper>
                        </Grow>
                        <Button className={this.props.classes.featuresButton} onClick={this.handleClick} id="about">LEARN MORE</Button>
                    </Paper>
                </Paper>
            </Router>
        );
    }
}

export default withStyles(styles)(withRouter(FrontPage));