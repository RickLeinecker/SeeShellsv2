import React from 'react';
import beach from '../assets/beach.png';
import pearl from '../assets/pearl.png';
import { withStyles } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';

const styles = {
    root: {
        display: 'flex',
        flexFlow: 'column wrap',
        justifyContent: 'center',
    },
    image: {
        display: 'flex',
        position: 'relative',
        width: '100%',
        height: '400px',
        textAlign: 'center',
        justifyContent: 'center',
        backgroundImage: 'linear-gradient(180deg, #FFFFFF 0%, rgba(255, 255, 255, 0) 100%), url(' + beach + ')',
    },
    downloadButton: {
        display: 'flex',
        position: 'absolute',
        backgroundColor: '#1D70EB',
        color: 'white',
        fontSize: '20px',
        margin: '0px',
        bottom: '10px',
    },
    intro: {
        display: 'flex',
        position: 'absolute',
        color: '#1D70EB',
        fontSize: '100px',
        margin: '0px',
        left: '10px',
    },
    description: {
        display: 'flex',
        position: 'absolute',
        color: 'black',
        fontSize: '40px',
        margin: '0px',
        left: '50px',
        top: '100px',
    },
    descriptionAlt: {
        color: '#1D70EB',
        fontWeight: 'bold',
        display: 'contents',
    },
    title: {
        color: 'black',
        fontSize: '70px',
        marginTop: '10px',
        marginBottom: '10px',
        textDecoration: 'underline',
    },
    infoContainer: {
        display: 'flex',
        justifyContent: 'center',
    },
    info: {
        display: 'flex',
        alignItems: 'center',
        fontSize: '30px',
        margin: '30px',
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
        backgroundColor: '#1D70EB',
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

// TODO: float the text and button over the image
class FrontPage extends React.Component {
    render() {
        return(
            <div className={this.props.classes.root}>
                <div className={this.props.classes.image}>
                    <div className={this.props.classes.image}/>
                    <p className={this.props.classes.intro}>SEESHELLS</p>
                    <p className={this.props.classes.description}>
                        A <span className={this.props.classes.descriptionAlt}>digital forensics tool</span> for analyzing Windows Registry Artifacts.
                    </p>
                    <Button className={this.props.classes.downloadButton}>GET THE TOOL</Button>
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
                    <Button className={this.props.classes.featuresButton}>SEE ALL FEATURES</Button>
                </div>
            </div>
        );
    }
}

export default withStyles(styles)(FrontPage);