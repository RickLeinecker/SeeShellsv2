import React from 'react';
import DeveloperProfile from '../components/DeveloperProfile.js';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';
import Typography from '@material-ui/core/Typography';

const styles = {
    devContainer: {
        display: 'flex',
        flexFlow: 'column',
        justifyContent: 'center',
        alignContent: 'center',
        width: '100%',
    },
    dev: {
        display: 'flex',
        height: '90%',
        justifyContent: 'space-evenly',
        flexWrap: 'wrap',
    },
    devIntro: {
        display: 'flex',
        justifyContent: 'center',
        flexDirection: 'column',
        alignItems: 'center',
    },
    title: {
        fontSize: '50px',
        fontWeight: 'bold',
        marginTop: '1%',
        alignSelf: 'center',
    },
};

class DeveloperPage extends React.Component {
    render() {
        return(
            <div className={this.props.classes.devContainer}>
                <div className={this.props.classes.devIntro}>
                    <Typography variant="title" className={this.props.classes.title}>SeeShells Development Teams</Typography>
                    <Typography variant="subtitle1">
                        The SeeShells application is a Senior Design project built by two teams of five Computer Science students at UCF.
                        The V1 team designed the original application, while the V2 team enhanced the project.
                    </Typography>
                </div>
                <div className={this.props.classes.dev}>
                    <DeveloperProfile name="Sara Frackiewicz" role="V1 Team: API, Scripting, and Administrative Website"/>
                    <DeveloperProfile name="Klayton Killough" role="V1 Team: WPF Shellbag Parser and IO"/>
                    <DeveloperProfile name="Aleksandar Stoyanov" role="V1 Team: WPF Shellbag Parser and Timeline"/>
                    <DeveloperProfile name="Bridget Woodye" role="V1 Team: WPF GUI and Timeline"/>
                    <DeveloperProfile name="Yara As-Saidi" role="V1 Team: WPF and Website Content"/>
                    <DeveloperProfile name="Devon Gadarowski" role="V2 Team: Shellbag Data Collection and Analysis"/>
                    <DeveloperProfile name="Kaylee Hoyt" role="V2 Team: Website Developer"/>
                    <DeveloperProfile name="Jake Meyer" role="V2 Team: WPF Timeline UI, Shellbag Export and Tagging"/>
                    <DeveloperProfile name="Spencer Ross" role="V2 Team:"/>
                    <DeveloperProfile name="Joshua Rueda" role="V2 Team: Video Producer"/>
                </div>
            </div>
        );
    }
}

export default withStyles(styles)(withRouter(DeveloperPage));