use std::fs;
use std::fs::File;
use std::io::Read;

use common::get_game_info;

use super::{Error, IoResult};

/// Different types of file that the module can load.
pub enum FileType {
    Savegame,
}

/// Reads the specified file from the file system and returns its content as a string.
pub fn read_file(file_type: FileType) -> IoResult<String> {
    let path = path_from_file_type(file_type)?;
    let mut file = File::open(path).map_err(|_err| Error::FileLoadingError)?;
    let mut contents = String::new();
    file.read_to_string(&mut contents)
        .map_err(|_err| Error::FileLoadingError)?;

    Ok(contents)
}

/// Saves the specified content in a file depending on the given FileType.
pub fn save_file(content: String, file_type: FileType) -> IoResult<()> {
    let path = path_from_file_type(file_type)?;
    fs::write(path, content).map_err(|_err| Error::FileWritingError)?;

    Ok(())
}

fn path_from_file_type(file: FileType) -> IoResult<String> {
    match file {
        FileType::Savegame => app_dirs2::app_root(app_dirs2::AppDataType::UserData, &app_info())
            .map(|buff| buff.into_os_string().into_string().unwrap() + "/savegame.json")
            .map_err(|_err| Error::PathFindingError),
    }
}

fn app_info() -> app_dirs2::AppInfo {
    let game_info = get_game_info();

    // The creators of app_dirs2 made the questionable choice of accepting only a &'static str when
    // creating the AppInfo struct. Since I don't want to hardcode the name of the app nor the
    // author and instead load them from the crate itself, there's no other choice but to leak the
    // string. TODO: Revisit this sometime in the future?
    app_dirs2::AppInfo {
        name: Box::leak(game_info.name.into_boxed_str()),
        author: Box::leak(game_info.author.into_boxed_str()),
    }
}
