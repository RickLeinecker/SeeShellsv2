import React from 'react';
import croppedLogo from '../assets/croppedLogo.png';
import croppedV2Logo from '../assets/croppedV2Logo.png';
import ucfLogo from '../assets/croppedUcfLogo.png';
import { withStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import Grow from '@material-ui/core/Grow';

const styles = {
    devContainer: {
        backgroundColor: '#424242',
        margin: '10px',
        width: '30%',
        height: '35%',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        flexDirection: 'column',
        color: 'white',
        minWidth: '350px',
        maxWidth: '425px',
        minHeight: '200px',
    },
    backgroundV1: {
        backgroundImage: 'linear-gradient(180deg, #424242 0%, rgba(255, 255, 255, 0) 100%), url(' + croppedLogo + ')',
        backgroundSize: 'cover',
        backgroundRepeat: 'no-repeat',
        width: '100%',
        height: '100%',
    },
    backgroundV2: {
        backgroundImage: 'linear-gradient(180deg, #424242 0%, rgba(255, 255, 255, 0) 100%), url(' + croppedV2Logo + ')',
        backgroundSize: 'cover',
        backgroundRepeat: 'no-repeat',
        width: '100%',
        height: '100%',
    },
    backgroundSponsor: {
        backgroundImage: 'linear-gradient(180deg, #424242 0%, rgba(255, 255, 255, 0) 100%), url(' + ucfLogo + ')',
        backgroundSize: 'cover',
        backgroundRepeat: 'no-repeat',
        width: '100%',
        height: '100%',
    },
    devTitle: {
        display: 'flex',
        justifyContent: 'right',
        textAlign: 'right',
        overflowWrap: 'break-word',
        color: '#33A1FD',
        marginTop: '10%',
        fontFamily: 'Georgia',
        fontWeight: 'bold',
    },
    devDescription: {
        display: 'flex',
        justifyContent: 'right',
        textAlign: 'right',
        overflowWrap: 'break-word',
    },
};

/*
*   DeveloperProfile.js
*   - component that displays the name and role for each person involved in SeeShells' development
*   - this.props.version === 1: V1 team members, renders a card with the V1 logo
*   - this.props.version === 2: V2 team members, renders a card with the V2 logo
*   - this.props.version === 0: SeeShells sponsor, renders a card with the UCF pegasus logo
*/
class DeveloperProfile extends React.Component {
    render() {
        return(
            <Grow in={true}>
                <Card className={this.props.classes.devContainer}>
                    {this.props.version === 1 &&
                        <div className={this.props.classes.backgroundV1}>
                            <CardContent>
                                <Typography variant="h5" className={this.props.classes.devTitle}>{this.props.name}</Typography>
                                <Typography variant="subtitle1" className={this.props.classes.devDescription}>{this.props.role}</Typography>
                            </CardContent>
                        </div>
                    }
                    {this.props.version === 2 &&
                        <div className={this.props.classes.backgroundV2}>
                            <CardContent>
                                <Typography variant="h5" className={this.props.classes.devTitle}>{this.props.name}</Typography>
                                <Typography variant="subtitle1" className={this.props.classes.devDescription}>{this.props.role}</Typography>
                            </CardContent>
                        </div>
                    }
                    {this.props.version === 0 &&
                        <div className={this.props.classes.backgroundSponsor}>
                            <CardContent>
                                <Typography variant="h5" className={this.props.classes.devTitle}>{this.props.name}</Typography>
                                <Typography variant="subtitle1" className={this.props.classes.devDescription}>{this.props.role}</Typography>
                            </CardContent>
                        </div>
                    }
                </Card>
            </Grow>
        );
    }
}

export default withStyles(styles)(DeveloperProfile);