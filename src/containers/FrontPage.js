import React from 'react';
import beach from '../assets/beach2.png';
import pearl from '../assets/pearl.png';
import { withStyles } from '@material-ui/core/styles';
import { withRouter, BrowserRouter as Router } from 'react-router-dom';
import Button from '@material-ui/core/Button';

const styles = {
    frontPage: {
        backgroundColor: '#F2F2F2',
        display: 'flex',
        flexFlow: 'column wrap',
        justifyContent: 'center',
        height: '100%',
    },
    image: {
        display: 'flex',
        position: 'relative',
        width: '100%',
        height: '350px',
        textAlign: 'center',
        justifyContent: 'center',
        backgroundImage: 'url(' + beach + ')',
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
    },
    barContent: {
        display: 'flex',
        flexFlow: 'column wrap',
    },
    intro: {
        display: 'flex',
        color: '#F2F2F2',
        fontSize: '100px',
        margin: '0px',
        left: '10px',
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
        fontSize: '70px',
        marginTop: '10px',
        marginBottom: '10px',
        textDecoration: 'underline',
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
        fontSize: '30px',
        margin: '30px',
        color: '#3E3E3E',
    },
    column: {
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
    },
    featuresContainer: {
        display: 'flex',
        justifyContent: 'center',
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
    }
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
            <Router>
                <div className={this.props.classes.frontPage}>
                    <div className={this.props.classes.image}>
                        <div className={this.props.classes.barContent}>
                            <p className={this.props.classes.intro}>SEESHELLS</p>
                            <p className={this.props.classes.description}>
                                A <span className={this.props.classes.descriptionAlt}>digital forensics tool</span> for analyzing Windows Registry Artifacts.
                            </p>
                        </div>
                        <Button className={this.props.classes.downloadButton} onClick={this.handleClick} id="download">GET THE TOOL</Button>
                    </div>
                    <div className={this.props.classes.featuresContainer}>
                        <p className={this.props.classes.title}>Why SeeShells?</p>
                    </div>
                    <div className={this.props.classes.infoContainer}>
                        <div className={this.props.classes.column}>
                            <div className={this.props.classes.info}>
                                <img src={pearl} alt='pearl' className={this.props.classes.pearl}/>
                                <span className={this.props.classes.descriptionAlt}>Quick parsing</span> of Windows Registry artifacts
                            </div>
                            <div className={this.props.classes.info}>
                                <img src={pearl} alt='pearl' className={this.props.classes.pearl}/>
                                <span className={this.props.classes.descriptionAlt}>Analysis of artifacts</span> with suspicious behavior flagging
                            </div>
                        </div>
                        <div className={this.props.classes.column}>
                            <div className={this.props.classes.info}>
                                <img src={pearl} alt='pearl' className={this.props.classes.pearl}/>
                                <span className={this.props.classes.descriptionAlt}>Timeline View</span> for a holistic view of computer activity
                            </div>
                            <div className={this.props.classes.info}>
                                <img src={pearl} alt='pearl' className={this.props.classes.pearl}/>
                                <span className={this.props.classes.descriptionAlt}>Filtering</span> for specific activities and trends
                            </div>
                        </div>
                    </div>
                    <div className={this.props.classes.featuresContainer}>
                        <Button className={this.props.classes.featuresButton} onClick={this.handleClick} id="about">SEE ALL FEATURES</Button>
                    </div>
                </div>
            </Router>
        );
    }
}

export default withStyles(styles)(withRouter(FrontPage));