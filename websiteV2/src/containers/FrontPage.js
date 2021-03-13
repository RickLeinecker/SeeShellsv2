import React from 'react';
import beach from '../assets/beach2.png';
import pearl from '../assets/pearl.png';
import '../assets/animation.css';
import { withStyles } from '@material-ui/core/styles';
import { withRouter, HashRouter as Router } from 'react-router-dom';
import Button from '@material-ui/core/Button';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';

const styles = {
    frontPage: {
        display: 'flex',
        flexFlow: 'column wrap',
        justifyContent: 'center',
        height: '100%',
        width: '100%',
        overflow: 'auto',
    },
    image: {
        display: 'grid',
        position: 'relative',
        width: '100%',
        height: '350px',
        textAlign: 'center',
        backgroundImage: 'url(' + beach + ')',
        overflow: 'hidden',
    },
    downloadButton: {
        display: 'flex',
        position: 'absolute',
        backgroundColor: '#33A1FD',
        '&:hover': {
            backgroundColor: '#EF476F',
        },
        color: 'white',
        fontSize: '20px',
        margin: '0px',
        bottom: '10px',
        justifySelf: 'center',
    },
    barContent: {
        display: 'flex',
        flexFlow: 'column wrap',
        justifySelf: 'center',
        paddingTop: '2%',
        paddingBottom: '10%',
    },
    intro: {
        display: 'flex',
        color: '#F2F2F2',
        fontSize: '100px',
        margin: '0px',
        left: '10px',
        paddingLeft: '10px',
    },
    description: {
        display: 'flex',
        color: '#F2F2F2',
        fontSize: '40px',
        margin: '0px',
        left: '50px',
        top: '100px',
        paddingLeft: '200px',
        paddingRight: '200px',
    },
    descriptionAlt: {
        color: '#33A1FD',
        fontWeight: 'bold',
        display: 'contents',
    },
    title: {
        color: '#082998',
        fontSize: '50px',
        marginTop: '1%',
        marginBottom: '1%',
        textDecoration: 'underline',
        fontWeight: 'bold',
    },
    infoContainer: {
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'flex-start',
        width: '70%',
        alignSelf: 'center',
    },
    info: {
        display: 'flex',
        alignItems: 'center',
        margin: '30px',
    },
    text: {
        fontSize: '20px',
    },
    column: {
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
    },
    featuresContainer: {
        display: 'flex',
        justifyContent: 'center',
        flexDirection: 'column',
        alignItems: 'center',
    },
    featuresButton: {
        display: 'flex',
        backgroundColor: '#33A1FD',
        '&:hover': {
            backgroundColor: '#EF476F',
        },
        color: 'white',
        fontSize: '20px',
        margin: '0px',
        justifyContent: 'center',
    },
    pearl: {
        height: '50px',
        width: '50px',
        verticalAlign: 'center',
        marginRight: '10px',
    },
};

class FrontPage extends React.Component {
    constructor(props) {
        super(props);

        this.handleClick = this.handleClick.bind(this);
    }

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
                        <div className={this.props.classes.barContent}>
                            <Typography className={this.props.classes.intro}>SEESHELLS</Typography>
                            <Typography className={this.props.classes.description}>
                                A <span className={this.props.classes.descriptionAlt}>digital forensics tool</span> for analyzing Windows Registry Artifacts.
                            </Typography>
                        </div>
                        <Button className={this.props.classes.downloadButton} onClick={this.handleClick} id="download">GET THE TOOL</Button>
                    </Paper>
                    <Paper elevation={0} className={this.props.classes.featuresContainer}>
                        <Typography className={this.props.classes.title}>Why SeeShells?</Typography>
                        <Paper elevation={0} className={this.props.classes.infoContainer}>
                            <Paper elevation={0} className={this.props.classes.column}>
                                <Paper elevation={0} className={this.props.classes.info}>
                                    <img src={pearl} alt='pearl' className={this.props.classes.pearl}/>
                                    <Typography className={this.props.classes.text}><span className={this.props.classes.descriptionAlt}>Quick parsing</span> of Windows Registry artifacts</Typography>
                                </Paper>
                                <Paper elevation={0} className={this.props.classes.info}>
                                    <img src={pearl} alt='pearl' className={this.props.classes.pearl}/>
                                    <Typography className={this.props.classes.text}><span className={this.props.classes.descriptionAlt}>Analysis of artifacts</span> with suspicious behavior flagging</Typography>
                                </Paper>
                            </Paper>
                            <Paper elevation={0} className={this.props.classes.column}>
                                <Paper elevation={0} className={this.props.classes.info}>
                                    <img src={pearl} alt='pearl' className={this.props.classes.pearl}/>
                                    <Typography className={this.props.classes.text}><span className={this.props.classes.descriptionAlt}>Timeline View</span> for a holistic view of computer activity</Typography>
                                </Paper>
                                <Paper elevation={0} className={this.props.classes.info}>
                                    <img src={pearl} alt='pearl' className={this.props.classes.pearl}/>
                                    <Typography className={this.props.classes.text}><span className={this.props.classes.descriptionAlt}>Filtering</span> for specific activities and trends</Typography>
                                </Paper>
                            </Paper>
                        </Paper>
                        <Paper elevation={0}>
                            <Button className={this.props.classes.featuresButton} onClick={this.handleClick} id="about">SEE ALL FEATURES</Button>
                        </Paper>
                    </Paper>
                </Paper>
            </Router>
        );
    }
}

export default withStyles(styles)(withRouter(FrontPage));