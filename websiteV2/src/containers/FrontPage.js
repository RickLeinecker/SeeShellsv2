import React from 'react';
import placeholder from '../assets/placeholder.png';
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
    },
    downloadButton: {
        display: 'flex',
        position: 'absolute',
        backgroundColor: '#1D70EB',
        color: 'white',
        fontSize: '20px',
        margin: '0px',
        bottom: '0',
    },
    intro: {
        display: 'flex',
        position: 'absolute',
        color: '#1D70EB',
        fontSize: '100px',
        margin: '0px',
        left: '10px',
    },
    introContainer: {

    },
    description: {
        display: 'flex',
        position: 'absolute',
        color: '#1D70EB',
        fontSize: '40px',
        margin: '0px',
        left: '50px',
        top: '100px',
    },
    infoContainer: {

    },
};

// TODO: float the text and button over the image
class FrontPage extends React.Component {
    render() {
        return(
            <div className={this.props.classes.root}>
                <div className={this.props.classes.image}>
                    <img src={placeholder} alt='placeholder' className={this.props.classes.image}/>
                    <p className={this.props.classes.intro}>SEESHELLS</p>
                    <p className={this.props.classes.description}>A digital forensics tool for analyzing Windows Registry Artifacts.</p>
                    <Button className={this.props.classes.downloadButton}>GET THE TOOL</Button>
                </div>
                <div className={this.props.classes.infoContainer}>
                </div>
            </div>
        );
    }
}

export default withStyles(styles)(FrontPage);