import React from 'react';
import beach from '../assets/beach2.png';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';

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
    login: {
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
    },
    title: {
        fontSize: '50px',
        fontWeight: 'bold',
    },
    field: {
        display: 'flex',
        flexDirection: 'column',
        color: 'white',
        fontSize: '20px',
        width: '80%',
        padding: '3%',
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
    register: {
        display: 'flex',
        backgroundColor: '#EF476F',
        '&:hover': {
            backgroundColor: '#33A1FD',
        },
        color: 'white',
        fontSize: '20px',
        margin: '0px',
        justifyContent: 'center',
        marginTop: '5%',
    },
    input: {
        backgroundColor: 'white',
    }
}

class DownloadPage extends React.Component {
    render() {
        return(
            <div className={this.props.classes.downloadPage}>
                <div className={this.props.classes.downloadContainer}>
                    <div className={this.props.classes.image}/>
                    <div className={this.props.classes.login}>
                        <p className={this.props.classes.title}>Download SeeShells</p>
                    </div>
                </div>
            </div>
        );
    }
}

export default withStyles(styles)(withRouter(DownloadPage));